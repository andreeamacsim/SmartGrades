import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';

interface Student {
  id: number;
  firstName: string;
  lastName: string;
}

interface Course {
  id: number;
  name: string;
}

interface ClassEnrollment {
  id: number;
  studentId: number;
  courseId: number;
}

@Component({
  selector: 'app-class-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './class-management.component.html',
  styleUrl: './class-management.component.css'
})
export class ClassManagementComponent implements OnInit {

  
  enrollmentForm: FormGroup;
  
  // Dummy Data - Same as teacher dashboard for consistency
  students: Student[] = [
    { id: 1, firstName: 'Andreea', lastName: 'Macsim' },
    { id: 2, firstName: 'Mihai', lastName: 'Moisescu' },
    { id: 3, firstName: 'Claudiu', lastName: 'Rusu' }
  ];
  
  courses: Course[] = [
    { id: 1, name: 'Mathematics' },
    { id: 2, name: 'Science' },
    { id: 3, name: 'English Literature' }
  ];
  
  classEnrollments: ClassEnrollment[] = [];
  selectedCourse: number | null = null;
  
  constructor(private fb: FormBuilder) {
    this.enrollmentForm = this.fb.group({
      studentId: ['', Validators.required],
      courseId: ['', Validators.required]
    });
  }
  
  ngOnInit(): void {
    // Initialize with some sample enrollments
    this.classEnrollments = [
      { id: 1, studentId: 1, courseId: 1 },
      { id: 2, studentId: 2, courseId: 1 },
      { id: 3, studentId: 1, courseId: 2 }
    ];
  }
  
  onSubmit() {
    if (this.enrollmentForm.valid) {
      const formValue = this.enrollmentForm.value;
      
      // Check if this enrollment already exists
      const existingEnrollment = this.classEnrollments.find(
        e => e.studentId === Number(formValue.studentId) && e.courseId === Number(formValue.courseId)
      );
      
      if (existingEnrollment) {
        alert('This student is already enrolled in this course!');
        return;
      }
      
      const newEnrollment: ClassEnrollment = {
        ...formValue,
        id: this.generateUniqueId(),
        studentId: Number(formValue.studentId),
        courseId: Number(formValue.courseId)
      };
      
      this.classEnrollments.push(newEnrollment);
      this.enrollmentForm.reset();
    }
  }
  
  generateUniqueId(): number {
    return this.classEnrollments.length > 0 
      ? Math.max(...this.classEnrollments.map(e => e.id)) + 1 
      : 1;
  }
  
  removeEnrollment(enrollment: ClassEnrollment) {
    const confirmDelete = confirm(`Are you sure you want to remove ${this.getStudentName(enrollment.studentId)} from ${this.getCourseName(enrollment.courseId)}?`);
    
    if (confirmDelete) {
      this.classEnrollments = this.classEnrollments.filter(e => e.id !== enrollment.id);
    }
  }
  
  getStudentName(studentId: number): string {
    const student = this.students.find(s => s.id === studentId);
    return student ? `${student.firstName} ${student.lastName}` : 'Unknown';
  }
  
  getCourseName(courseId: number): string {
    const course = this.courses.find(c => c.id === courseId);
    return course ? course.name : 'Unknown';
  }
  
  filterByCourse(courseId: number | null) {
    this.selectedCourse = courseId;
  }
  
  getEnrolledStudents() {
    if (this.selectedCourse === null) {
      return this.classEnrollments;
    }
    return this.classEnrollments.filter(e => e.courseId === this.selectedCourse);
  }


  onCourseFilterChange(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    const value = selectElement.value;
    this.filterByCourse(value ? +value : null);
  }
}