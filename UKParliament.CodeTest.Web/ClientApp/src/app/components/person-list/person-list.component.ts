import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { PersonViewModel } from 'src/app/models/person-view-model';
import { PersonService } from 'src/app/services/person.service';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-person-list',
  standalone: true,
  imports: [CommonModule, ButtonModule, TableModule],
  templateUrl: './person-list.component.html',
  styleUrl: './person-list.component.scss',
})
export class PersonListComponent implements OnInit, OnDestroy {
  constructor(
    private readonly personService: PersonService,
    private readonly router: Router
  ) {}

  persons: PersonViewModel[] = [];
  destroy$: Subject<void> = new Subject<void>();

  ngOnInit(): void {
    this.personService
      .getAll()
      .pipe(takeUntil(this.destroy$))
      .subscribe((persons) => {
        this.persons = persons;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  addPerson(): void {
    this.router.navigate(['/add']);
  }

  editPerson(id: number): void {
    this.router.navigate(['/edit', id]);
  }

  deletePerson(id: number): void {
    this.personService
      .delete(id)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.persons = this.persons.filter((p) => p.id !== id);
      });
  }
}
