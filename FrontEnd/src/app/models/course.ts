import { Student } from "./student";
import { Teacher } from "./teacher";

export interface Course{
    id:string,
    name:string,
    students:Student[],
    teachers:Teacher[],
}