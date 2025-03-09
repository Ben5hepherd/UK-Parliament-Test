import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { takeUntil } from 'rxjs';
import { PersonViewModel } from 'src/app/models/person-view-model';
import { DropdownModule } from 'primeng/dropdown';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { AddEditPersonComponent } from '../add-edit-person/add-edit-person.component';

@Component({
  selector: 'app-add-person',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    DropdownModule,
    DatePickerModule,
    ButtonModule,
  ],
  templateUrl: '../add-edit-person/add-edit-person.component.html',
  styleUrl: '../add-edit-person/add-edit-person.component.scss',
})
export class AddPersonComponent extends AddEditPersonComponent {
  title: string = 'Add New Person';

  override onSubmit(): void {
    this.personForm.markAllAsTouched();

    if (this.personForm.invalid) {
      return;
    }

    const newPerson: PersonViewModel = {
      id: 0,
      firstName: this.personForm.controls.firstName.value,
      lastName: this.personForm.controls.lastName.value,
      dateOfBirth: this.personForm.controls.dateOfBirth.value,
      department: {
        id: this.personForm.controls.department.value,
        name: '',
      },
    };

    this.personService
      .add(newPerson)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.router.navigate(['']);
      });
  }
}
