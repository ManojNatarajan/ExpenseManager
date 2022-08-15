import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

//component imports
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './layout/header/header.component';
import { FooterComponent } from './layout/footer/footer.component';
import { SigninComponent } from './pages/signin/signin.component';
import { SignupComponent } from './pages/signup/signup.component';
import { HomeComponent } from './pages/home/home.component';
import { PagenotfoundComponent } from './pages/pagenotfound/pagenotfound.component';
import { ExpensesummarylistComponent } from './components/expensesummarylist/expensesummarylist.component';
import { MonthlyexpenseentrieslistComponent } from './components/monthlyexpenseentrieslist/monthlyexpenseentrieslist.component';

//forms
import { FormsModule } from "@angular/forms";
import { ReactiveFormsModule } from '@angular/forms';

//for toastr
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { ToastrModule } from "ngx-toastr";

//Jwt Module
import { JwtModule } from "@auth0/angular-jwt";
import { AuthGuard } from './guards/auth-guard.service';

//HttpClientModule & HttpInterceptor
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { AuthInterceptor } from './guards/AuthInterceptor';
import { ExpensetypeconfigComponent } from './pages/expensetypeconfig/expensetypeconfig.component';

//Social Login 
import { SocialLoginModule, SocialAuthServiceConfig } from '@abacritt/angularx-social-login';
import {
  GoogleLoginProvider,
  FacebookLoginProvider
} from '@abacritt/angularx-social-login';

//Icons
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ExpenseentryComponent } from './pages/expenseentry/expenseentry.component';
import { DatePipe } from '@angular/common';

export function tokenGetter() {
  return localStorage.getItem("jwt");
}

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    SigninComponent,
    SignupComponent,
    HomeComponent,
    PagenotfoundComponent,
    ExpensesummarylistComponent,
    MonthlyexpenseentrieslistComponent,
    ExpensetypeconfigComponent,
    ExpenseentryComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,    
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    SocialLoginModule,
    FontAwesomeModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:7053"],
        disallowedRoutes: []
      }
    })
  ],
  providers: [        
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(
              // google client id
              '169249215209-ou8h8r52p59bel1gplvu8ngjk43e6bib.apps.googleusercontent.com'
            )
          },
          {
            id: FacebookLoginProvider.PROVIDER_ID,
            provider: new FacebookLoginProvider(
              //Facebook App id 
              '1569476446780714', { prompt: 'select_account', auth_type: 'reauthenticate',}
              ),
          }
        ],
        onError: (err: any) => {
          console.error(err);
        },
      } as SocialAuthServiceConfig,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    AuthGuard,
    DatePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
