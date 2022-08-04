import { TestBed } from '@angular/core/testing';

import { ExpensemanagerauthService } from './expensemanagerauth.service';

describe('ExpensemanagerauthService', () => {
  let service: ExpensemanagerauthService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ExpensemanagerauthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
