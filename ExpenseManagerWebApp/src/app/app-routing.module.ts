import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './pages/home/home.component';
import { PagenotfoundComponent } from './pages/pagenotfound/pagenotfound.component';
import { SigninComponent } from './pages/signin/signin.component';
import { SignupComponent } from './pages/signup/signup.component';
import { ExpensetypeconfigComponent } from './pages/expensetypeconfig/expensetypeconfig.component';

import { AuthGuard } from './guards/auth-guard.service';

const routes: Routes = [  
  { 
    path: 'signin', 
    component: SigninComponent,        
  },
  {
    path: 'signup',
    component: SignupComponent
  },
  {
    path: '',
    component: HomeComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'expensetypeconfig',
    component: ExpensetypeconfigComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "**",
    component: PagenotfoundComponent
  }  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
