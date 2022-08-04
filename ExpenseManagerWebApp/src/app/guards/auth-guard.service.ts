import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoggerService } from '../common/logger.service';
import { LocalService } from './../common/local.service';

@Injectable()
export class AuthGuard implements CanActivate{

    constructor(
        private jwtHelper: JwtHelperService, 
        private router: Router,
        private localStore: LocalService,
        private logger: LoggerService){
    }

    canActivate(){
        const token = this.localStore.getData("jwt");

        if (token && !this.jwtHelper.isTokenExpired(token)){
            this.logger.Info(`canActivate: True`);
            return true;
        }

        this.logger.Info(`canActivate: False`);
        this.router.navigateByUrl('/signin');
        return false;
    }


}