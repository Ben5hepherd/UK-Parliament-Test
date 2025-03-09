import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { DepartmentViewModel } from 'src/app/models/department-view-model';
import { PersonViewModel } from 'src/app/models/person-view-model';
import { DepartmentService } from 'src/app/services/department.service';
import { PersonService } from 'src/app/services/person.service';

@Component({
  selector: 'app-person-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './person-list.component.html',
  styleUrl: './person-list.component.scss',
})
export class PersonListComponent implements OnInit, OnDestroy {
  constructor(
    private readonly personService: PersonService,
    private readonly departmentService: DepartmentService
  ) {}

  persons: PersonViewModel[] = [];
  departments: DepartmentViewModel[] = [];

  newPersonId: number = 0;

  destroy$: Subject<void> = new Subject<void>();

  ngOnInit(): void {
    this.personService.getAll().subscribe((persons) => {
      this.persons = persons;
    });

    this.departmentService
      .getAll()
      .pipe(takeUntil(this.destroy$))
      .subscribe((departments) => {
        this.departments = departments;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  addPerson(): void {
    let p: PersonViewModel = {
      firstName: 'John',
      lastName: 'Doe',
      dateOfBirth: new Date(1990, 1, 1),
      department: this.departments[0],
    };

    this.personService.add(p).subscribe((personId) => {
      this.newPersonId = personId;
    });
  }

  updatePerson(): void {
    this.persons[0].firstName = 'Jane';

    this.personService.update(this.persons[0]).pipe(takeUntil(this.destroy$)).subscribe();
  }

  deletePerson(): void {
    this.personService.delete(1).pipe(takeUntil(this.destroy$)).subscribe();
  }
}
