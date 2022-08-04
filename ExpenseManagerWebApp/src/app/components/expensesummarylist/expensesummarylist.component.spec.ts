import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpensesummarylistComponent } from './expensesummarylist.component';

describe('ExpensesummarylistComponent', () => {
  let component: ExpensesummarylistComponent;
  let fixture: ComponentFixture<ExpensesummarylistComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExpensesummarylistComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExpensesummarylistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
