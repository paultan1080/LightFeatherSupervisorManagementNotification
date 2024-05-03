import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SupervisorService } from './supervisor.service';
import { HttpClientModule } from '@angular/common/http';
import { NotificationFormModel } from './notification-form.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    HttpClientModule,
    ReactiveFormsModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  notificationForm!: FormGroup;
  supervisors: string[] = [];

  // Server-side validation errors
  serverSideFormErrors: string[] = [];
  isFormSubmmittedSuccessfully = false;

  constructor(
    private fb: FormBuilder,
    private supervisorService: SupervisorService
  ) {
    this.notificationForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: [{ value: '', disabled: true }],
      phoneNumber: [{ value: '', disabled: true }],
      supervisor: ['', Validators.required],
      emailEnabled: [false],
      phoneEnabled: [false]
    });

    // Reset server-side errors when the form is touched
    this.notificationForm.valueChanges.subscribe(() => {
      if (this.serverSideFormErrors.length > 0) {
        this.serverSideFormErrors = [];
      }
    });

    // Listeners to enable/disable email and phone based on checkbox
    this.notificationForm.get('emailEnabled')!.valueChanges.subscribe(checked => {
      const emailControl = this.notificationForm.get('email');
      if (checked) {
        emailControl!.enable();
        emailControl!.setValidators([Validators.required, Validators.email]);
      } else {
        emailControl!.disable();
        emailControl!.clearValidators();
      }

      emailControl!.updateValueAndValidity();
    });

    this.notificationForm.get('phoneEnabled')!.valueChanges.subscribe(checked => {
      const phoneControl = this.notificationForm.get('phoneNumber');
      if (checked) {
        phoneControl!.enable();
        phoneControl!.setValidators([Validators.required]);
      } else {
        phoneControl!.disable();
        phoneControl!.clearValidators();
      }

      phoneControl!.updateValueAndValidity();
    });
  }

  ngOnInit() {
    this.supervisorService.getSupervisors().subscribe({
      next: (data) => {
        this.supervisors = data;
      },
      error: (err) => console.error('Failed to fetch supervisors:', err)
    });

  }

  onSubmit() {
    if (this.notificationForm.valid) {
      const formData: NotificationFormModel = this.notificationForm.value;

      if (!this.notificationForm.get('emailEnabled')!) {
        delete formData["email"];
      }

      if (!this.notificationForm.get('phoneEnabled')!) {
        delete formData["phoneNumber"];
      }

      this.supervisorService.submitForm(formData).subscribe({
        next: () => {
          this.notificationForm.reset();
          this.isFormSubmmittedSuccessfully = true;
        },
        error: (errorMessage: string) => {
          this.serverSideFormErrors = [];

          if (errorMessage) {
            this.serverSideFormErrors = (errorMessage + '').split('\n');
          }
        }
      });
    }
  }
}
