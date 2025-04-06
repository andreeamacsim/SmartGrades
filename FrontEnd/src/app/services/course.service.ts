import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Course } from '../models/course';
import { Student } from '../models/student';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class CourseService {
  basesurl='https://localhost:7261/course'
  constructor(private httpClient:HttpClient) {
   }
   getTeacherCourses(teacherId:string)
   {
    return this.httpClient.get<Course[]>(`${this.basesurl}/teacher-id?id=${teacherId}`);
   }
   getStudentsFromCourse(courseId:string)
   {
    return this.httpClient.get<Student[]>(`${this.basesurl}/students-from-course?courseId=${courseId}`);
   }
   getStudentCourses(studentId: string) {
    return this.httpClient.get<Course[]>(`${this.basesurl}/student-courses?studentId=${studentId}`);
  }
  addStudentToCourse(studentId: string, courseId: string): Observable<void> {
    return this.httpClient.post<void>(`${this.basesurl}/enroll/${courseId}/${studentId}`, null);
  }  
  removeStudentFromCourse(studentId: string, courseId: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.basesurl}/unenroll/${courseId}/${studentId}`);
  }
} 
