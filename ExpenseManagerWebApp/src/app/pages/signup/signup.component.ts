import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PasswordChecker } from './../../common/custom-validators/password-checker';

import { AlertService } from 'src/app/common/alert.service';
import { LoggerService } from 'src/app/common/logger.service';

import { User } from 'src/app/models/user';
import { ExpensemanagerauthService } from 'src/app/services/expensemanagerauth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  title = "signup";  
  submitted = false;
  signupForm: FormGroup;
  user: User;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private logger: LoggerService,
    private alert: AlertService,
    private auth: ExpensemanagerauthService
    ) { }    
 
  ngOnInit(): void {
    this.signupForm = this.formBuilder.group(
      {
        name: ["", Validators.required],
        mobile: ["", [Validators.required, Validators.maxLength(10), Validators.minLength(10)]],
        email: ["", [Validators.required, Validators.email]],
        password: ["", [Validators.required, Validators.minLength(6)]],
        confirmPassword: ["", Validators.required],
        acceptTandC: [false, Validators.requiredTrue]
      }, 
      {
        validators: PasswordChecker("password","confirmPassword"),
      }
    );
  }

  get ctrl(){
    return this.signupForm.controls;
  }

  onSubmit(){
    this.submitted = true;
    if(this.signupForm.invalid){      
      this.alert.error("Signup Validation Failed.")
      return;
    }

    this.logger.debugTable(this.signupForm);
    this.logger.debug(JSON.stringify(this.signupForm.value));

    this.user = new User();
    this.user.UserName = this.signupForm.controls['name'].value;
    this.user.Mobile = this.signupForm.controls['mobile'].value;
    this.user.Email = this.signupForm.controls['email'].value;
    this.user.Password = this.signupForm.controls['password'].value;
    this.user.Userstatusid = 1;    
    this.user.AcceptTandC = this.signupForm.controls['acceptTandC'].value;

    this.logger.debugTable(this.user);

    this.auth.signupUser(this.user).subscribe({
      next: (response) => {
        this.logger.Info(`Signup Response from API: ${response}`)
        this.router.navigateByUrl('/signin');
        this.alert.success("Signup Successful! Please Login to continue. ");
      },
      error: (err) => {
        this.logger.UnknownWithMessage(err, 'Signup Failed with API Error.')
      },
      complete: () => this.logger.Info('Signup Complete!')
    });
  }

  onReset(){
    this.submitted = false;
    this.signupForm.reset();
  }

}
