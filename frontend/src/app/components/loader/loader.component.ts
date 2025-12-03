import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoaderService } from '../../services/loader.service';

@Component({
    selector: 'app-loader',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './loader.template.html',
    styleUrl: './loader.style.css'
})
export class LoaderComponent {
    constructor(public loaderService: LoaderService) { }
}