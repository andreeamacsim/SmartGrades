import { CommonModule } from '@angular/common';
import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Grade } from '../models/grade';
import { Teacher } from '../models/teacher';
import { TeacherService } from '../services/teacher.service';
import { GradeService } from '../services/grade.service';
import { Course } from '../models/course';
import { CourseService } from '../services/course.service';
import { AuthService } from '../services/auth.service';
import { Student } from '../models/student';


@Component({
  selector: 'app-teacher-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './teacher-dashboard.component.html',
  styleUrl: './teacher-dashboard.component.css'
})
export class TeacherDashboardComponent implements OnInit {
  gradeEntryForm: FormGroup;
  students: Student[] = [];
  courses: Course[] = [];
  recentGrades: Grade[] = [];
  editingGrade: Grade | null = null;
  private teacherId='';
  
  constructor(
    private fb: FormBuilder,
    private teacherService: TeacherService,
    private courseService: CourseService,
    protected authService: AuthService
  ) {
    this.teacherId=this.authService.connectedUserId;
    this.gradeEntryForm = this.fb.group({
      studentId: ['', Validators.required],
      courseId: ['', Validators.required],
      assignmentName: ['', [Validators.required, Validators.minLength(3)]],
      score: ['', [Validators.required, Validators.min(0)]],
      maxGrade: ['', [Validators.required, Validators.min(1)]],
      gradedDate: ['', Validators.required]
    });
  }
  
  ngOnInit(): void {
    // Load teacher's courses
    this.courseService.getTeacherCourses(this.teacherId).subscribe({
      next: courses => {
        this.courses = courses;
        console.log(courses);
      },
      error: err => {
        console.error("Error fetching courses:", err);
      }
    });
    
    // Load teacher's recent grades
    this.loadRecentGrades(this.teacherId);
    
    // Subscribe to course changes to load students
    this.gradeEntryForm.get('courseId')?.valueChanges.subscribe((courseId) => {
      if (courseId) {
        this.courseService.getStudentsFromCourse(courseId).subscribe({
          next: (students) => {
            this.students = students; 
          },
          error: (err) => {
            console.error("Error fetching students for the course:", err);
          }
        });
      }
    });
  }
  
  loadRecentGrades(teacherId: string): void {
    this.teacherService.getTeacherGrades(this.teacherId).subscribe({
      next: (grades) => {
        this.recentGrades = grades;
      },
      error: (err) => {
        console.error("Error fetching recent grades:", err);
      }
    });
  }
  
  onSubmit() {
    if (this.gradeEntryForm.valid) {
      const formValue = this.gradeEntryForm.value;
      
      if (this.editingGrade) {
        // Update existing grade
        const updatedGrade: Grade = {
          ...this.editingGrade,
          ...formValue,
          gradedDate: new Date(formValue.gradedDate)
        };
        
        this.teacherService.updateGrade(updatedGrade).subscribe({
          next: (success) => {
            if (success) {
              const index = this.recentGrades.findIndex(g => g.id === updatedGrade.id);
              if (index !== -1) {
                this.recentGrades[index] = updatedGrade;
              }
              this.editingGrade = null;
              this.gradeEntryForm.reset();
            }
          },
          error: (err) => {
            console.error("Error updating grade:", err);
          }
        });
      } else {
        // Add new grade
        const newGrade: Grade = {
          ...formValue,
          id: '',
          teacherId: this.teacherId,
          gradedDate: new Date(formValue.gradedDate)
        };
        
        this.teacherService.addGrade(newGrade).subscribe({
          next: (success) => {
            if (success) {
              // Reload grades after successful addition
              this.loadRecentGrades(this.teacherId);
              this.gradeEntryForm.reset();
            }
          },
          error: (err) => {
            console.error("Error adding grade:", err);
          }
        });
      }
    }
  }
  
  editGrade(grade: Grade) {
    this.editingGrade = grade;
    
    this.gradeEntryForm.patchValue({
      studentId: grade.studentId,
      courseId: grade.courseId,
      assignmentName: grade.assignmentName,
      score: grade.score,
      maxGrade: grade.maxGrade,
      gradedDate: grade.gradedDate instanceof Date 
        ? grade.gradedDate.toISOString().split('T')[0] 
        : new Date(grade.gradedDate).toISOString().split('T')[0]
    });
  }
  
  deleteGrade(grade: Grade) {
    const studentName = this.getStudentName(grade.studentId);
    const courseName = this.getCourseName(grade.courseId);
    const confirmDelete = confirm(`Are you sure you want to delete the grade for ${studentName} in ${courseName}?`);
    
    if (confirmDelete) {
      this.teacherService.deleteGrade(grade.id).subscribe({
        next: (success) => {
          if (success) {
            this.recentGrades = this.recentGrades.filter(g => g.id !== grade.id);
            
            if (this.editingGrade?.id === grade.id) {
              this.editingGrade = null;
              this.gradeEntryForm.reset();
            }
          }
        },
        error: (err) => {
          console.error("Error deleting grade:", err);
        }
      });
    }
  }
  
  getStudentName(studentId: string): string {
    const student = this.students.find(s => s.id === studentId);
    return student ? `${student.username}` : 'Unknown';
  }
  
  getCourseName(courseId: string): string {
    const course = this.courses.find(c => c.id === courseId);
    return course ? course.name : 'Unknown';
  }
  
  cancelEdit() {
    this.editingGrade = null;
    this.gradeEntryForm.reset();
  }
}