import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthlyexpenseentrieslistComponent } from './monthlyexpenseentrieslist.component';

describe('MonthlyexpenseentrieslistComponent', () => {
  let component: MonthlyexpenseentrieslistComponent;
  let fixture: ComponentFixture<MonthlyexpenseentrieslistComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MonthlyexpenseentrieslistComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MonthlyexpenseentrieslistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
