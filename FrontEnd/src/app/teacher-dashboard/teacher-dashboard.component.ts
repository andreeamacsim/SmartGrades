import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Grade } from '../models/grade';
import { Teacher } from '../models/teacher';
import { TeacherService } from '../services/teacher.service';
import { GradeService } from '../services/grade.service';

interface Student {
  id: number;
  firstName: string;
  lastName: string;
}

interface Course {
  id: number;
  name: string;
}



@Component({
  selector: 'app-teacher-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './teacher-dashboard.component.html',
  styleUrl: './teacher-dashboard.component.css'
})
export class TeacherDashboardComponent implements OnInit {
  gradeEntryForm: FormGroup;
  
  // Dummy Data --TODO : add/ edit/delete the data from database
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
  
  recentGrades: Grade[] = [];
  
  editingGrade: Grade | null = null;
  
  constructor(private fb: FormBuilder ,private teacherService:TeacherService,private gradeService:GradeService) {
    this.gradeEntryForm = this.fb.group({
      studentId: ['', Validators.required],
      courseId: ['', Validators.required],
      assignmentName: ['', [Validators.required, Validators.minLength(3)]],
      score: ['', [Validators.required, Validators.min(0)]],
      maxScore: ['', [Validators.required, Validators.min(1)]],
      gradedDate: ['', Validators.required]
    });
  }
  
  ngOnInit(): void {
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