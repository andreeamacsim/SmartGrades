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
export class TeacherDashboardComponent implements OnInit{
  gradeEntryForm: FormGroup;
  students:Student[] = [];
  
  courses: Course[] = [];
  
  recentGrades: Grade[] = [];
  
  editingGrade: Grade | null = null;
  
  constructor(private fb: FormBuilder ,private teacherService:TeacherService,private courseService:CourseService,private authService:AuthService) {
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
    const teacherId = this.authService.authenticatedUser?.id;
    if (!teacherId) {
      console.error("Teacher ID is undefined.");
      return;
    }
    
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

    this.courseService.getTeacherCourses(teacherId).subscribe({
      next: courses => {
        this.courses = courses;
        console.log(courses);
      },
      error: err => {
        console.error("Error fetching courses:", err);
      }
    });
   }
  
  onSubmit() {
    if (this.gradeEntryForm.valid) {
      const formValue = this.gradeEntryForm.value;
      
      if (this.editingGrade) {
        const index = this.recentGrades.findIndex(g => g.id === this.editingGrade?.id);
        if (index !== -1) {
          this.recentGrades[index] = {
            ...this.editingGrade,
            ...formValue,
            gradedDate: new Date(formValue.gradedDate)
          };
        }
        this.editingGrade = null;
      } else {
   
        const newGrade: Grade = {
          ...formValue,
          id:'',
          teacherId:this.authService.authenticatedUser?.id,
          gradedDate: new Date(formValue.gradedDate)
        };
        this.teacherService.addGrade(newGrade).subscribe();
      }
      
      this.gradeEntryForm.reset();
    }
  }
  

  // generateUniqueId(): number {
  //   return this.recentGrades.length > 0 
  //     ? Math.max(...this.recentGrades.map(g => g.id || 0)) + 1 
  //     : 1;
  // }
  
  
  // editGrade(grade: Grade) {
  //   this.editingGrade = grade;
    

  //   this.gradeEntryForm.patchValue({
  //     studentId: grade.studentId,
  //     courseId: grade.courseId,
  //     assignmentName: grade.assignmentName,
  //     score: grade.score,
  //     maxScore: grade.maxGrade,
  //     gradedDate: grade.gradedDate.toISOString().split('T')[0] 
  //   });
  // }
  
 
  // deleteGrade(grade: Grade) {
  //   const confirmDelete = confirm(`Are you sure you want to delete the grade for ${this.getStudentName(grade.studentId)} in ${this.getCourseName(grade.courseId)}?`);
    
  //   if (confirmDelete) {
  //     this.recentGrades = this.recentGrades.filter(g => g.id !== grade.);
      

  //     if (this.editingGrade?.id === grade.id) {
  //       this.editingGrade = null;
  //       this.gradeEntryForm.reset();
  //     }
  //   }
  // }
  

  // getStudentName(studentId: string): string {
  //   const student = this.students.find(s => s.id === studentId);
  //   return student ? `${student.firstName} ${student.lastName}` : 'Unknown';
  // }
  
  // getCourseName(courseId: number): string {
  //   const course = this.courses.find(c => c.id === courseId);
  //   return course ? course.name : 'Unknown';
  // }
  
 
  cancelEdit() {
    this.editingGrade = null;
    this.gradeEntryForm.reset();
  }
}