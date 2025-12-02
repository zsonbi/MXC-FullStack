import { Component, OnInit, ChangeDetectorRef } from '@angular/core'; 
import { CommonModule } from '@angular/common';
import { EventService } from '../../services/event.service';
import { EventDto } from '../../models/api-models';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-event-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './event-list.component.html'
})
export class EventListComponent implements OnInit {
  events: EventDto[] = [];
  sortDirection: { [key: string]: 'asc' | 'desc' } = {};

  constructor(
    private eventService: EventService,
    private cdr: ChangeDetectorRef 
  ) {}

  ngOnInit(): void {
    this.loadEvents();
  }

  loadEvents() {
    this.eventService.getEvents().subscribe({
      next: (data) => {
        this.events = data;
        // FORCE the view to update immediately
        this.cdr.detectChanges(); 
      },
      error: (err) => console.error('Error loading events:', err)
    });
  }


deleteEvent(id: string) {
  if (confirm('Are you sure you want to delete this event?')) {
    this.eventService.deleteEvent(id).subscribe({
      next: () => {

        this.loadEvents(); 

      },
      error: (err) => {
        console.error('Error on deletion:', err);
        alert('Couldn\'t delete the event, could it be that it no longer exists?');
      }
    });
  }
}
  
  // Sort logic for the columns
  sort(property: keyof EventDto) {
    const direction = this.sortDirection[property] === 'asc' ? 'desc' : 'asc';
    this.sortDirection[property] = direction;

    this.events.sort((a: any, b: any) => {
      const valueA = a[property];
      const valueB = b[property];

      if (valueA < valueB) return direction === 'asc' ? -1 : 1;
      if (valueA > valueB) return direction === 'asc' ? 1 : -1;
      return 0;
    });
  }
}