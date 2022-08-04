import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoggerService } from '../common/logger.service';
import { LocalService } from './../common/local.service';

@Injectable({
    providedIn: 'root'
  })
export class TokenHelper{

    constructor(
        private jwtHelper: JwtHelperService,
        private logger: LoggerService,
        private localStore: LocalService
        ){        
    }

    getUserIdClaim(): number{
        let token: any;
        token = this.localStore.getData("jwt");
        let decoded = this.jwtHelper.decodeToken(token);
        this.logger.Info('Claim Value: ' + decoded.UserId);
        return decoded.UserId;
    }

}