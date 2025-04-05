import { Grade } from "../grade";
import { CourseGradesDto } from "./CourseGradesDto";

export interface DashboardSummaryDto {
    courseGrades: CourseGradesDto[];
    overallGPA: number;
  }