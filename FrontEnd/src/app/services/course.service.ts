import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Course } from '../models/course';
import { Student } from '../models/student';

@Injectable({
  providedIn: 'root'
})
export class CourseService {
  basesurl='https://localhost:7261/Course'
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
} 
