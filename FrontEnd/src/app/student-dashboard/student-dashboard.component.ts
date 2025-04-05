import { Component, OnInit } from '@angular/core'; 
import { CommonModule } from '@angular/common'; 
import { GradeService } from '../services/grade.service';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Grade } from '../models/grade';
import { CourseService } from '../services/course.service';
import { Course } from '../models/course'; // Import Course model

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
  styleUrls: ['./student-dashboard.component.css']
})
export class StudentDashboardComponent implements OnInit {

  courseGrades: CourseGrades[] = [];
  overallGPA: number = 0;
  private studentId: string;
  courses: Course[] = [];  // New property to store student courses

  constructor(
    private gradeService: GradeService,
    private router: Router,
    protected authService: AuthService,
    private courseService: CourseService // Inject CourseService
  ) {
    this.studentId = this.authService.connectedUserId;
  }

  ngOnInit(): void {
    this.loadStudentCourses(); // Load courses first
  }

  loadStudentCourses() {
    this.courseService.getStudentCourses(this.studentId).subscribe({
      next: (courses) => {
        this.courses = courses;
        this.loadStudentGrades(); // Then load grades
      },
      error: (err) => {
        console.error('Failed to load student courses:', err);
      }
    });
  }

  loadStudentGrades() {
    this.gradeService.getStudentGrades(this.studentId).subscribe({
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
      if (!courseMap.has(grade.courseId)) {
        courseMap.set(grade.courseId, []);
      }
      courseMap.get(grade.courseId)!.push(grade);
    });

    this.courseGrades = Array.from(courseMap.entries()).map(([courseId, courseGrades]) => {
      const courseAverage = this.calculateCourseAverage(courseGrades);
      const courseName = this.courses.find(c => c.id === courseId)?.name || courseId;

      return {
        courseId,
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
    const totalMaxScore = grades.reduce((sum, grade) => sum + grade.maxGrade, 0);
    
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
