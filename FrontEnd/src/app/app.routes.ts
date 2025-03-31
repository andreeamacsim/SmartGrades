import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { StudentDashboardComponent } from './student-dashboard/student-dashboard.component';
import { TeacherDashboardComponent } from './teacher-dashboard/teacher-dashboard.component';
import { ClassManagementComponent } from './class-management/class-management.component';
import { TeacherProfileComponent } from './teacher-profile/teacher-profile.component';
import { StudentProfileComponent } from './student-profile/student-profile.component';


export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent},
  { path: 'student-dashboard', component:StudentDashboardComponent},
  { path: 'teacher-dashboard', component:TeacherDashboardComponent},
  { path: 'class-management', component: ClassManagementComponent },
  { path: 'teacher-profile', component: TeacherProfileComponent },
  { path: 'student-profile', component:StudentProfileComponent}

];