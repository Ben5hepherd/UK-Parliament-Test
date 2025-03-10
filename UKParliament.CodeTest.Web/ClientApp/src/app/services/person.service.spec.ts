import { TestBed } from '@angular/core/testing';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { PersonService } from './person.service';
import { PersonViewModel } from '../models/person-view-model';
import { DepartmentViewModel } from '../models/department-view-model';
import { HttpClient, provideHttpClient } from '@angular/common/http';

describe('PersonService', () => {
  let service: PersonService;
  let httpMock: HttpTestingController;
  let baseUrl: string;

  beforeEach(() => {
    baseUrl = 'baseURL';

    TestBed.configureTestingModule({
      providers: [
        { provide: 'BASE_URL', useValue: baseUrl },
        HttpClient,
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    });

    service = TestBed.inject(PersonService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getById', () => {
    it('should return a person by id', () => {
      const mockPerson: PersonViewModel = {
        id: 1,
        firstName: 'John',
        lastName: 'Doe',
        emailAddress: 'test@test.com',
        dateOfBirth: new Date('1990-01-01'),
        department: { id: 1, name: 'IT' } as DepartmentViewModel,
      };

      service.getById(1).subscribe((person) => {
        expect(person).toEqual(mockPerson);
      });

      const req = httpMock.expectOne(`${baseUrl}api/person/1`);
      expect(req.request.method).toBe('GET');
      req.flush(mockPerson);
    });
  });

  describe('getAll', () => {
    it('should return an array of persons', () => {
      const mockPersons: PersonViewModel[] = [
        {
          id: 1,
          firstName: 'John',
          lastName: 'Doe',
          dateOfBirth: new Date('1990-01-01'),
          emailAddress: 'test@test.com',
          department: { id: 1, name: 'IT' } as DepartmentViewModel,
        },
        {
          id: 2,
          firstName: 'Jane',
          lastName: 'Smith',
          emailAddress: 'test2@test.com',
          dateOfBirth: new Date('1992-02-15'),
          department: { id: 2, name: 'HR' } as DepartmentViewModel,
        },
      ];

      service.getAll().subscribe((persons) => {
        expect(persons).toEqual(mockPersons);
      });

      const req = httpMock.expectOne(`${baseUrl}api/person`);
      expect(req.request.method).toBe('GET');
      req.flush(mockPersons);
    });
  });

  describe('add', () => {
    it('should add a person and return the id', () => {
      const newPerson: PersonViewModel = {
        id: 0,
        firstName: 'John',
        lastName: 'Doe',
        emailAddress: 'test@test.com',
        dateOfBirth: new Date('1990-01-01'),
        department: { id: 1, name: 'IT' } as DepartmentViewModel,
      };
      const expectedId = 1;

      service.add(newPerson).subscribe((id) => {
        expect(id).toBe(expectedId);
      });

      const req = httpMock.expectOne(`${baseUrl}api/person`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(newPerson);
      req.flush(expectedId);
    });
  });

  describe('update', () => {
    it('should update a person', () => {
      const updatedPerson: PersonViewModel = {
        id: 1,
        firstName: 'John',
        lastName: 'Doe Updated',
        emailAddress: 'test@test.com',
        dateOfBirth: new Date('1990-01-01'),
        department: { id: 1, name: 'IT' } as DepartmentViewModel,
      };

      service.update(updatedPerson).subscribe((response) => {
        expect(response).toBeNull();
      });

      const req = httpMock.expectOne(`${baseUrl}api/person`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual(updatedPerson);
      req.flush(null);
    });
  });

  describe('delete', () => {
    it('should delete a person', () => {
      const personId = 1;

      service.delete(personId).subscribe((response) => {
        expect(response).toBeNull();
      });

      const req = httpMock.expectOne(`${baseUrl}api/person/1`);
      expect(req.request.method).toBe('DELETE');
      req.flush(null);
    });
  });
});
