import { Grade } from "./grade";


export interface Student{
    id:string;
    username:string;
    password:string;
    email:string;
    courseIds:string[];
    grades:Grade[];
}