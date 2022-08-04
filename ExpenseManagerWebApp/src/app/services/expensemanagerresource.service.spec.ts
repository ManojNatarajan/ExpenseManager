import { TestBed } from '@angular/core/testing';

import { ExpensemanagerresourceService } from './expensemanagerresource.service';

describe('ExpensemanagerresourceService', () => {
  let service: ExpensemanagerresourceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ExpensemanagerresourceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
