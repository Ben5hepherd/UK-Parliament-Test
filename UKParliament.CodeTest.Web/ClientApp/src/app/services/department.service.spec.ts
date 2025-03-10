import { TestBed } from '@angular/core/testing';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { DepartmentService } from './department.service';
import { DepartmentViewModel } from '../models/department-view-model';
import { HttpClient, provideHttpClient } from '@angular/common/http';

describe('DepartmentService', () => {
  let service: DepartmentService;
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

    service = TestBed.inject(DepartmentService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getAll', () => {
    it('should return all departments', () => {
      const mockDepartments: DepartmentViewModel[] = [
        { id: 1, name: 'Department 1' },
        { id: 2, name: 'Department 2' },
      ];

      service.getAll().subscribe((departments) => {
        expect(departments).toEqual(mockDepartments);
      });

      const req = httpMock.expectOne(`${baseUrl}api/department`);
      expect(req.request.method).toBe('GET');
      req.flush(mockDepartments);
    });
  });
});
