import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Category } from '../_models/Category';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})
export class CategoryService {
    private baseUrl: string = environment.baseUrl + 'api/';

    constructor(private http: HttpClient) { }

    public addCategory(category: Category) {
        return this.http.post(this.baseUrl + 'Categories', category);
    }

    public updateCategory(id: number, category: Category) {
        return this.http.put(this.baseUrl + 'categories/' + id, category);
    }

    public getCategories(): Observable<Category[]> {
        return this.http.get<Category[]>(this.baseUrl + `categories`);
    }

    public deleteCategory(id: number) {
        return this.http.delete(this.baseUrl + 'categories/' + id);
    }

    public getCategoryById(id): Observable<Category> {
        return this.http.get<Category>(this.baseUrl + 'categories/' + id);
    }

    public search(name: string): Observable<Category[]> {
        return this.http.get<Category[]>(`${this.baseUrl}categories/search/${name}`);
    }
}
