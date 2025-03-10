import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PersonListComponent } from './person-list.component';
import { PersonService } from 'src/app/services/person.service';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { PersonViewModel } from 'src/app/models/person-view-model';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';

describe('PersonListComponent', () => {
  let component: PersonListComponent;
  let fixture: ComponentFixture<PersonListComponent>;
  let personServiceMock: jasmine.SpyObj<PersonService>;
  let routerMock: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    personServiceMock = jasmine.createSpyObj('PersonService', [
      'getAll',
      'delete',
    ]);
    routerMock = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [CommonModule, ButtonModule, TableModule],
      providers: [
        { provide: PersonService, useValue: personServiceMock },
        { provide: Router, useValue: routerMock },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(PersonListComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load persons on init', () => {
    const mockPersons: PersonViewModel[] = [
      {
        id: 1,
        firstName: 'John',
        lastName: 'Doe',
        dateOfBirth: new Date(),
        department: { id: 1, name: 'HR' },
      },
    ];
    personServiceMock.getAll.and.returnValue(of(mockPersons));

    fixture.detectChanges();

    expect(component.persons.length).toBe(1);
    expect(component.persons[0].firstName).toBe('John');
  });

  it('should navigate to add person page when addPerson is called', () => {
    component.addPerson();

    expect(routerMock.navigate).toHaveBeenCalledWith(['/add']);
  });

  it('should navigate to edit person page when editPerson is called', () => {
    component.editPerson(1);

    expect(routerMock.navigate).toHaveBeenCalledWith(['/edit', 1]);
  });

  it('should delete a person when deletePerson is called', () => {
    const mockPersons: PersonViewModel[] = [
      {
        id: 1,
        firstName: 'John',
        lastName: 'Doe',
        dateOfBirth: new Date(),
        department: { id: 1, name: 'HR' },
      },
      {
        id: 2,
        firstName: 'Jane',
        lastName: 'Smith',
        dateOfBirth: new Date(),
        department: { id: 2, name: 'IT' },
      },
    ];
    component.persons = [...mockPersons];
    personServiceMock.delete.and.returnValue(of(null));

    component.deletePerson(1);

    expect(component.persons.length).toBe(1);
    expect(component.persons[0].id).toBe(2);
  });
});
