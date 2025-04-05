import { Grade } from "../grade";

export interface CourseGradesDto {
    courseId: string;  
    courseName: string;
    grades: Grade[];
    courseAverage: number;
  }