import { Component } from '@angular/core';
import { takeUntil } from 'rxjs';
import { PersonViewModel } from 'src/app/models/person-view-model';
import { AddEditPersonComponent } from '../add-edit-person/add-edit-person.component';
import { AddEditPersonModule } from '../add-edit-person/add-edit-person.module';

@Component({
  selector: 'app-edit-person',
  standalone: true,
  imports: [AddEditPersonModule],
  templateUrl: '../add-edit-person/add-edit-person.component.html',
})
export class EditPersonComponent extends AddEditPersonComponent {
  title: string = 'Edit';

  ngOnInit(): void {
    super.ngOnInit();
    this.personId = Number(this.activatedRoute.snapshot.paramMap.get('id'));

    if (this.personId) {
      this.personService
        .getById(this.personId)
        .pipe(takeUntil(this.destroy$))
        .subscribe((person) => {
          this.title += ` ${person.firstName} ${person.lastName}`;
          this.personForm.patchValue({
            firstName: person.firstName,
            lastName: person.lastName,
            dateOfBirth: new Date(person.dateOfBirth),
            emailAddress: person.emailAddress,
            department: person.department.id,
          });
        });
    }
  }

  override onSubmit(): void {
    this.personForm.markAllAsTouched();

    if (this.personForm.invalid) {
      return;
    }

    const updatedPerson: PersonViewModel = {
      id: this.personId,
      firstName: this.personForm.controls.firstName.value!,
      lastName: this.personForm.controls.lastName.value!,
      dateOfBirth: this.personForm.controls.dateOfBirth.value!,
      emailAddress: this.personForm.controls.emailAddress.value!,
      department: {
        id: this.personForm.controls.department.value!,
        name: '',
      },
    };

    this.personService
      .update(updatedPerson)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.router.navigate(['']);
      });
  }
}
