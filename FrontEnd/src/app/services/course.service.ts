import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Course } from '../models/course';

@Injectable({
  providedIn: 'root'
})
export class CourseService {
  basesurl='https://localhost:7261/Course'
  constructor(private httpClient:HttpClient) {
   }
   getTeacherCourses(teacherId:string)
   {
    return this.httpClient.get<Course[]>(`${this.basesurl}/id?id=${teacherId}`);
   }
}
