export interface Grade{
    id:string,
    courseId:string,
    teacherId:string,
    studentId:string,
    assignmentName:string,
    score:number,
    maxGrade:number,
    gradedDate:Date,
}