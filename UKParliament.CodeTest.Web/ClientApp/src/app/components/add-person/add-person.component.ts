import { Component } from '@angular/core';
import { takeUntil } from 'rxjs';
import { PersonViewModel } from 'src/app/models/person-view-model';
import { AddEditPersonComponent } from '../add-edit-person/add-edit-person.component';
import { AddEditPersonModule } from '../add-edit-person/add-edit-person.module';

@Component({
  selector: 'app-add-person',
  standalone: true,
  imports: [AddEditPersonModule],
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
      firstName: this.personForm.controls.firstName.value!,
      lastName: this.personForm.controls.lastName.value!,
      emailAddress: this.personForm.controls.emailAddress.value!,
      dateOfBirth: this.personForm.controls.dateOfBirth.value!,
      department: {
        id: this.personForm.controls.department.value!,
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
