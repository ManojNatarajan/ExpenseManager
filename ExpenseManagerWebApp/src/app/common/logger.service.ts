import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoggerService {

  constructor() { }

  debug(message: string){
    console.log(message);
  }

  debugTable(tabularData?: any){
    console.table(tabularData);
  }

  debugUnknown(any: any){
    console.log(any);
  }

  debugUnknownWithMessage(any: any, message: string){
    console.log(any);
    console.log(message);
  }

  Info(message: string){
    console.log(message);
  }

  Warning(message: string){
    console.log(message);
  }

  Error(err: Error){
    console.log(err);    
  }

  ErrorMessage(message: string){
    console.log(message);
  }

  ErrorWithMessage(err: Error, message: string){
    console.log(err);
    console.log(message);
  }

 Unknown(any: any){
    console.log(any);
  }

  UnknownWithMessage(any: any, message: string){
    console.log(any);
    console.log(message);
  }

  Table(tabularData?: any){
    console.table(tabularData);
  }

}
