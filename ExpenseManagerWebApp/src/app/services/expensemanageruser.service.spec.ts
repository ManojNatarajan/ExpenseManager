import { TestBed } from '@angular/core/testing';

import { ExpensemanageruserService } from './expensemanageruser.service';

describe('ExpensemanageruserService', () => {
  let service: ExpensemanageruserService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ExpensemanageruserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
