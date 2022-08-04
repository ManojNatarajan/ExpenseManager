import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpensetypeconfigComponent } from './expensetypeconfig.component';

describe('ExpensetypeconfigComponent', () => {
  let component: ExpensetypeconfigComponent;
  let fixture: ComponentFixture<ExpensetypeconfigComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExpensetypeconfigComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExpensetypeconfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
