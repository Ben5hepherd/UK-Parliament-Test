import { DepartmentViewModel } from "./department-view-model";

export interface PersonViewModel {
  firstName: string;
  lastName: string;
  dateOfBirth: Date;
  department: DepartmentViewModel;
}
