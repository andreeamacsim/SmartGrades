import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CourseService } from '../services/course.service';
import { TeacherService } from '../services/teacher.service';
import { AuthService } from '../services/auth.service';
import { Course } from '../models/course';
import { Student } from '../models/student';
import { Teacher } from '../models/teacher';
import { StudentService } from '../services/student.service';

@Component({
  selector: 'app-class-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './class-management.component.html',
  styleUrl: './class-management.component.css'
})
export class ClassManagementComponent implements OnInit {

  enrollmentForm: FormGroup;
  students: Student[] = [];
  courses: Course[] = [];
  private teacherId='';
  enrolledStudentsMap: Map<string, Set<string>> = new Map();
  selectedCourse: string | null = null;
  
  constructor(private fb: FormBuilder,
              private studentService: StudentService,
              private teacherService: TeacherService,
              private courseService: CourseService,
              protected authService: AuthService
  ) {
    this.teacherId=this.authService.connectedUserId;
    this.enrollmentForm = this.fb.group({
      studentId: ['', Validators.required],
      courseId: ['', Validators.required]
    });
  }
  
  ngOnInit(): void {
    // Get the courses for the teacher
    this.courseService.getTeacherCourses(this.teacherId).subscribe({
      next: courses => {
        this.courses = courses;
        // For each course, fetch the enrolled students and populate the map
        courses.forEach(course => {
          // Fetch the enrolled students for the course using CourseService
          this.courseService.getStudentsFromCourse(course.id).subscribe({
            next: students => {
              // Create a set of student IDs for this course
              const studentIds = new Set<string>();
              students.forEach(student => {
                studentIds.add(student.id);
              });
              // Store the student IDs in the map with the course ID
              this.enrolledStudentsMap.set(course.id, studentIds);
            },
            error: err => {
              console.error("Error fetching students for course", course.id, err);
            }
          });
        });
      },
      error: err => {
        console.error("Error fetching courses:", err);
      }
    });
  
    // Get all students for other purposes (e.g., for the form)
    this.studentService.getAllStudents().subscribe({
      next: students => {
        this.students = students;
      },
      error: err => {
        console.error("Error fetching all students:", err);
      }
    });
  }
  
  onSubmit() {
    console.log(this.enrollmentForm.valid); // Log form validity to check
    if (this.enrollmentForm.valid) {
      const formValue = this.enrollmentForm.value;
      const studentId = formValue.studentId;
      const courseId = formValue.courseId;
      
      // Check if this student is already enrolled in the course
      if (this.isStudentEnrolledInCourse(studentId, courseId)) {
        alert('This student is already enrolled in this course!');
        return;
      }
  
      // Now add the student to the course
      this.addStudentToCourse(studentId, courseId);
      this.enrollmentForm.reset();
    }
  }


  isStudentEnrolledInCourse(studentId: string, courseId: string): boolean {
    return this.enrolledStudentsMap.has(courseId) &&
      this.enrolledStudentsMap.get(courseId)!.has(studentId);
  }

  addStudentToCourse(studentId: string, courseId: string): void {
  if (!this.enrolledStudentsMap.has(courseId)) {
    this.enrolledStudentsMap.set(courseId, new Set());
  }
  this.enrolledStudentsMap.get(courseId)!.add(studentId);

  // Update the enrollment in the backend via the CourseService
  this.courseService.addStudentToCourse(studentId, courseId).subscribe({
    next: () => {
      console.log(`Student ${studentId} added to course ${courseId}`);
    },
    error: err => {
      console.error("Error adding student to course", err);
    }
  });
}
  
  removeEnrollment(studentId: string, courseId: string): void {
    const confirmDelete = confirm(`Are you sure you want to remove ${this.getStudentName(studentId)} from ${this.getCourseName(courseId)}?`);
  
    if (confirmDelete) {
      // Remove the student from the enrolled list
      this.enrolledStudentsMap.get(courseId)?.delete(studentId);
  
      // Update the database
      this.courseService.removeStudentFromCourse(studentId, courseId).subscribe({
        next: () => {
          console.log(`Student ${studentId} removed from course ${courseId}`);
        },
        error: err => {
          console.error("Error removing student from course", err);
        }
      });
    }
  }
  
  getStudentName(studentId: string): string {
    const student = this.students.find(s => s.id === studentId);  // Compare as strings
    return student ? student.username : 'Unknown';
  }
  
  getCourseName(courseId: string): string {
    // Check if a course is selected
    if (!courseId) {
      return 'Unknown';
    }
  
    const course = this.courses.find(c => c.id === courseId); // Compare as strings
    return course ? course.name : 'Unknown';
  }
  
  
  filterByCourse(courseId: string | null) {
    this.selectedCourse = courseId;
  }
  
  getAllEnrolledStudents(): Student[] {
    const allEnrolledStudents: Student[] = [];
  
    // Iterate through each course in the map
    this.enrolledStudentsMap.forEach((studentIds, courseId) => {
      // For each student ID in the set, find the corresponding student and add them to the list
      studentIds.forEach(studentId => {
        const student = this.students.find(s => s.id === studentId);
        if (student) {
          allEnrolledStudents.push(student);
        }
      });
    });
  
    return allEnrolledStudents;
  }

  onCourseFilterChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const value = selectElement.value;
    this.filterByCourse(value || null);
  }
}