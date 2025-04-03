import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { StudentDashboardComponent } from './student-dashboard/student-dashboard.component';
import { TeacherDashboardComponent } from './teacher-dashboard/teacher-dashboard.component';
import { ClassManagementComponent } from './class-management/class-management.component';
import { TeacherProfileComponent } from './teacher-profile/teacher-profile.component';
import { StudentProfileComponent } from './student-profile/student-profile.component';
import { authGuard } from './guards/auth.guard';


export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent},
<<<<<<< Updated upstream
  { path: 'student-dashboard', component:StudentDashboardComponent,canActivate:[authGuard]},
  { path: 'teacher-dashboard', component:TeacherDashboardComponent,canActivate:[authGuard]},
  { path: 'class-management', component: ClassManagementComponent ,canActivate:[authGuard]},
  { path: 'teacher-profile', component: TeacherProfileComponent,canActivate:[authGuard]},
  { path: 'student-profile', component:StudentProfileComponent ,canActivate:[authGuard]}
=======
  { path: 'reset-password', component: ResetPasswordComponent},
  { path: 'student-dashboard', component:StudentDashboardComponent},
  { path: 'teacher-dashboard', component:TeacherDashboardComponent},
  { path: 'class-management', component: ClassManagementComponent },
  { path: 'teacher-profile', component: TeacherProfileComponent },
  { path: 'student-profile', component:StudentProfileComponent}
>>>>>>> Stashed changes

];