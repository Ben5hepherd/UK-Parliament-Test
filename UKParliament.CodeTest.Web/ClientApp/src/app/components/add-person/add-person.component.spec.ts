import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddPersonComponent } from './add-person.component';
import { PersonService } from 'src/app/services/person.service';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';
import { PersonViewModel } from 'src/app/models/person-view-model';
import { AddEditPersonModule } from '../add-edit-person/add-edit-person.module';
import { ReactiveFormsModule } from '@angular/forms';
import { DepartmentService } from 'src/app/services/department.service';
import { DatePipe } from '@angular/common';

describe('AddPersonComponent', () => {
  let component: AddPersonComponent;
  let fixture: ComponentFixture<AddPersonComponent>;
  let personServiceMock: jasmine.SpyObj<PersonService>;
  let departmentServiceMock: jasmine.SpyObj<DepartmentService>;
  let routerMock: jasmine.SpyObj<Router>;
  let activatedRouteMock: any;

  const mockDepartments = [
    { id: 1, name: 'HR' },
    { id: 2, name: 'IT' },
  ];

  beforeEach(async () => {
    personServiceMock = jasmine.createSpyObj('PersonService', ['add']);
    departmentServiceMock = jasmine.createSpyObj('DepartmentService', [
      'getAll',
    ]);
    routerMock = jasmine.createSpyObj('Router', ['navigate']);
    activatedRouteMock = {
      snapshot: {
        paramMap: {
          get: jasmine.createSpy('get').and.returnValue('1'),
        },
      },
    };

    await TestBed.configureTestingModule({
      imports: [AddEditPersonModule, ReactiveFormsModule, DatePipe],
      providers: [
        { provide: PersonService, useValue: personServiceMock },
        { provide: DepartmentService, useValue: departmentServiceMock },
        { provide: Router, useValue: routerMock },
        { provide: ActivatedRoute, useValue: activatedRouteMock },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AddPersonComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should submit new person details and navigate away', () => {
    component.personForm.setValue({
      firstName: 'Jane',
      lastName: 'Doe',
      dateOfBirth: new Date('1995-05-05'),
      department: 2,
    });

    personServiceMock.add.and.returnValue(of(1));
    departmentServiceMock.getAll.and.returnValue(of(mockDepartments));

    component.onSubmit();

    const expectedmodel: PersonViewModel = {
      id: 0,
      firstName: 'Jane',
      lastName: 'Doe',
      dateOfBirth: new Date('1995-05-05'),
      department: {
        id: 2,
        name: '',
      },
    };

    expect(personServiceMock.add).toHaveBeenCalledOnceWith(expectedmodel);
    expect(routerMock.navigate).toHaveBeenCalledWith(['']);
  });

  it('should not submit if form is invalid', () => {
    component.personForm.setValue({
      firstName: '',
      lastName: '',
      dateOfBirth: '',
      department: '',
    });

    component.onSubmit();

    expect(personServiceMock.add).not.toHaveBeenCalled();
    expect(routerMock.navigate).not.toHaveBeenCalled();
  });
});
