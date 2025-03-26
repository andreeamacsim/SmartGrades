import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

interface Grade {
  courseId: number;
  courseName: string;
  assignmentName: string;
  score: number;
  maxScore: number;
  gradedDate: Date;
}

@Injectable({
  providedIn: 'root'
})
export class GradeService {
  // dummy data just to see if the service works
  private mockGrades: Grade[] = [
    {
      courseId: 1,
      courseName: 'Mathematics',
      assignmentName: 'Algebra Midterm',
      score: 85,
      maxScore: 100,
      gradedDate: new Date('2024-03-15')
    },
    {
      courseId: 1,
      courseName: 'Mathematics',
      assignmentName: 'Calculus Quiz',
      score: 78,
      maxScore: 100,
      gradedDate: new Date('2024-03-22')
    },
    {
      courseId: 2,
      courseName: 'Computer Science',
      assignmentName: 'Programming Project',
      score: 92,
      maxScore: 100,
      gradedDate: new Date('2024-03-18')
    },
    {
      courseId: 2,
      courseName: 'Computer Science',
      assignmentName: 'Data Structures Exam',
      score: 88,
      maxScore: 100,
      gradedDate: new Date('2024-03-25')
    },
    {
      courseId: 3,
      courseName: 'English Literature',
      assignmentName: 'Essay Analysis',
      score: 90,
      maxScore: 100,
      gradedDate: new Date('2024-03-20')
    }
  ];

  constructor() { }

  // Method to get student grades (will be replaced with API call later)
  getStudentGrades(): Observable<Grade[]> {
    
    return of(this.mockGrades);
  }
}