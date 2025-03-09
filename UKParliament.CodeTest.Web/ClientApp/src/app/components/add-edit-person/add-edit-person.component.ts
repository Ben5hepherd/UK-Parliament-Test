import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { DepartmentViewModel } from 'src/app/models/department-view-model';
import { DepartmentService } from 'src/app/services/department.service';
import { PersonService } from 'src/app/services/person.service';
import { AddEditPersonModule } from './add-edit-person.module';

@Component({
  selector: 'app-edit-person',
  standalone: true,
  imports: [AddEditPersonModule],
  templateUrl: './add-edit-person.component.html',
  styleUrls: ['./add-edit-person.component.scss'],
})
export abstract class AddEditPersonComponent implements OnInit, OnDestroy {
  personForm: FormGroup;
  departments: DepartmentViewModel[] = [];
  personId: number = 0;
  destroy$: Subject<void> = new Subject<void>();

  title: string = '';

  constructor(
    private readonly fb: FormBuilder,
    protected readonly router: Router,
    protected readonly activatedRoute: ActivatedRoute,
    private readonly departmentService: DepartmentService,
    protected readonly personService: PersonService
  ) {
    this.personForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      dateOfBirth: ['', Validators.required],
      department: [null, Validators.required],
    });
  }

  ngOnInit(): void {
    this.departmentService
      .getAll()
      .pipe(takeUntil(this.destroy$))
      .subscribe((departments) => (this.departments = departments));
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  abstract onSubmit(): void;
}
