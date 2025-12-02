import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { EventService } from '../../services/event.service';

@Component({
    selector: 'app-event-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, RouterModule],
    templateUrl: './event-form.component.html'
})
export class EventFormComponent implements OnInit {
    eventForm: FormGroup;
    isEditMode = false;
    eventId: string | null = null;
    pageTitle = 'New event';

    constructor(
        private fb: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private eventService: EventService
    ) {
        this.eventForm = this.fb.group({
            id: [null], 
            name: ['', Validators.required],
            location: ['', Validators.required,Validators.maxLength(100)], 
            country: ['', Validators.required],
            capacity: [null, [Validators.required, Validators.min(1)]]
        });
    }

    ngOnInit(): void {
        this.eventId = this.route.snapshot.paramMap.get('id');
        if (this.eventId) {
            this.isEditMode = true;
            this.pageTitle = 'Edit event';
            this.eventService.getEvent(this.eventId).subscribe(event => {
                this.eventForm.patchValue(event);
            });
        }
    }

    onSubmit() {
        if (this.eventForm.invalid) return;

        const eventData: any = { ...this.eventForm.value };


        if (!this.isEditMode || !eventData.id) {
            delete eventData.id;
        }

        if (this.isEditMode) {
            this.eventService.updateEvent(eventData).subscribe({
                next: () => this.router.navigate(['/events']),
                error: (err) => console.error('Error during editing:', err)
            });
        } else {
            this.eventService.createEvent(eventData).subscribe({
                next: () => this.router.navigate(['/events']),
                error: (err) => console.error('Error during creation:', err)
            });
        }
    }
}