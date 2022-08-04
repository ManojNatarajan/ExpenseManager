import { TestBed } from '@angular/core/testing';

import { ExpensetypeService } from './expensetype.service';

describe('ExpensetypeService', () => {
  let service: ExpensetypeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ExpensetypeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
