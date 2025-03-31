import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { GradeService } from '../services/grade.service';
import { Router } from '@angular/router';

interface Grade {
  courseId: number;
  courseName: string;
  assignmentName: string;
  score: number;
  maxScore: number;
  gradedDate: Date;
}

interface CourseGrades {
  courseName: string;
  grades: Grade[];
  courseAverage: number;
}

@Component({
  selector: 'app-student-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './student-dashboard.component.html',
  styleUrl: './student-dashboard.component.css'
})
export class StudentDashboardComponent implements OnInit {

  // TODO--- backend for get assignment scores, calculate current average
  courseGrades: CourseGrades[] = [];
  overallGPA: number = 0;

  constructor(private gradeService: GradeService, private router: Router) {}

  ngOnInit(): void {
    this.loadStudentGrades();
  }

  loadStudentGrades() {
    this.gradeService.getStudentGrades().subscribe({
      next: (grades: Grade[]) => {
        this.processGrades(grades);
      },
      error: (error) => {
        console.error('Error fetching grades', error);
      }
    });
  }

  processGrades(grades: Grade[]) {
    const courseMap = new Map<string, Grade[]>();
    
    grades.forEach(grade => {
      if (!courseMap.has(grade.courseName)) {
        courseMap.set(grade.courseName, []);
      }
      courseMap.get(grade.courseName)!.push(grade);
    });

    this.courseGrades = Array.from(courseMap.entries()).map(([courseName, courseGrades]) => {
      const courseAverage = this.calculateCourseAverage(courseGrades);
      return {
        courseName,
        grades: courseGrades,
        courseAverage
      };
    });

    this.overallGPA = this.calculateOverallGPA();
  }

  calculateCourseAverage(grades: Grade[]): number {
    if (grades.length === 0) return 0;
    
    const totalScore = grades.reduce((sum, grade) => sum + grade.score, 0);
    const totalMaxScore = grades.reduce((sum, grade) => sum + grade.maxScore, 0);
    
    return totalScore / totalMaxScore * 100;
  }

  calculateOverallGPA(): number {
    if (this.courseGrades.length === 0) return 0;
    
    const averages = this.courseGrades.map(course => course.courseAverage);
    return averages.reduce((sum, avg) => sum + avg, 0) / averages.length;
  }

  navigateToStudentProfile() {
    this.router.navigate(['/student-profile']);
  }
}