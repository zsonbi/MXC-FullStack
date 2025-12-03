import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class LoaderService {
    // BehaviorSubject holds the current value (true/false)
    isLoading = new BehaviorSubject<boolean>(true);
    show() {
        this.isLoading.next(true);
    }

    hide() {
        this.isLoading.next(false);
    }
}