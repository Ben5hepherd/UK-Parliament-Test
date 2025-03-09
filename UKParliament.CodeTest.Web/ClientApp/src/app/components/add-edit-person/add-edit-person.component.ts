import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { PersonViewModel } from 'src/app/models/person-view-model';
import { DepartmentViewModel } from 'src/app/models/department-view-model';
import { DepartmentService } from 'src/app/services/department.service';
import { PersonService } from 'src/app/services/person.service';
import { DropdownModule } from 'primeng/dropdown';
import { DatePickerModule } from 'primeng/datepicker';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-edit-person',
  standalone: true,
  imports: [
    CommonModule,
    DropdownModule,
    DatePickerModule,
    ButtonModule,
    FormsModule,
    ReactiveFormsModule
  ],
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

  abstract onSubmit(): void
}
