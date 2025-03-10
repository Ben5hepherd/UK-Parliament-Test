import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
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
  personForm: FormGroup<PersonForm>;
  departments: DepartmentViewModel[] = [];
  personId: number = 0;
  destroy$: Subject<void> = new Subject<void>();

  title: string = '';

  constructor(
    protected readonly router: Router,
    protected readonly activatedRoute: ActivatedRoute,
    private readonly departmentService: DepartmentService,
    protected readonly personService: PersonService
  ) {
    this.personForm = new FormGroup<PersonForm>({
      firstName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      emailAddress: new FormControl('', [
        Validators.required,
        Validators.email,
      ]),
      dateOfBirth: new FormControl(null, Validators.required),
      department: new FormControl(null, Validators.required),
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

interface PersonForm {
  firstName: FormControl<string | null>;
  lastName: FormControl<string | null>;
  emailAddress: FormControl<string | null>;
  dateOfBirth: FormControl<Date | null>;
  department: FormControl<number | null>;
}
