import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { StudentDashboardComponent } from './student-dashboard/student-dashboard.component';
import { TeacherDashboardComponent } from './teacher-dashboard/teacher-dashboard.component';


export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent},
  { path: 'student-dashboard', component:StudentDashboardComponent},
  { path: 'teacher-dashboard', component:TeacherDashboardComponent}

];