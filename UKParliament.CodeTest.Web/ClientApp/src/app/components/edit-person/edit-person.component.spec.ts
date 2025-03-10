import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EditPersonComponent } from './edit-person.component';
import { PersonService } from 'src/app/services/person.service';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';
import { PersonViewModel } from 'src/app/models/person-view-model';
import { AddEditPersonModule } from '../add-edit-person/add-edit-person.module';
import { ReactiveFormsModule } from '@angular/forms';
import { DepartmentService } from 'src/app/services/department.service';
import { DatePipe } from '@angular/common';

describe('EditPersonComponent', () => {
  let component: EditPersonComponent;
  let fixture: ComponentFixture<EditPersonComponent>;
  let personServiceMock: jasmine.SpyObj<PersonService>;
  let departmentServiceMock: jasmine.SpyObj<DepartmentService>;
  let routerMock: jasmine.SpyObj<Router>;
  let activatedRouteMock: any;

  const mockDepartments = [
    { id: 1, name: 'HR' },
    { id: 2, name: 'IT' },
  ];

  beforeEach(async () => {
    personServiceMock = jasmine.createSpyObj('PersonService', [
      'getById',
      'update',
    ]);
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

    fixture = TestBed.createComponent(EditPersonComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load person details on init', () => {
    const mockPerson: PersonViewModel = {
      id: 1,
      firstName: 'John',
      lastName: 'Doe',
      emailAddress: 'test@test.com',
      dateOfBirth: new Date('1990-01-01'),
      department: { id: 1, name: 'HR' },
    };

    personServiceMock.getById.and.returnValue(of(mockPerson));
    departmentServiceMock.getAll.and.returnValue(of(mockDepartments));

    fixture.detectChanges();

    expect(component.title).toBe('Edit John Doe');
    expect(component.personForm.value.firstName).toBe('John');
  });

  it('should submit updated person details and navigate away', () => {
    component.personId = 1;
    component.personForm.setValue({
      firstName: 'Jane',
      lastName: 'Doe',
      dateOfBirth: new Date('1995-05-05'),
      emailAddress: 'test@test.com',
      department: 2,
    });

    personServiceMock.update.and.returnValue(of(null));
    departmentServiceMock.getAll.and.returnValue(of(mockDepartments));

    component.onSubmit();

    const expectedModel = {
      id: 1,
      firstName: 'Jane',
      lastName: 'Doe',
      emailAddress: 'test@test.com',
      dateOfBirth: new Date('1995-05-05'),
      department: { id: 2, name: '' },
    };

    expect(personServiceMock.update).toHaveBeenCalledOnceWith(expectedModel);
    expect(routerMock.navigate).toHaveBeenCalledWith(['']);
  });

  it('should not submit if form is invalid', () => {
    component.personForm.setValue({
      firstName: '',
      lastName: '',
      emailAddress: '',
      dateOfBirth: null,
      department: null,
    });

    component.onSubmit();

    expect(personServiceMock.update).not.toHaveBeenCalled();
    expect(routerMock.navigate).not.toHaveBeenCalled();
  });
});
