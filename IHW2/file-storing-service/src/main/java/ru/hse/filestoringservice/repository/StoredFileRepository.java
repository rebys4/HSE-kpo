package ru.hse.filestoringservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import ru.hse.filestoringservice.entity.StoredFile;

public interface StoredFileRepository extends JpaRepository<StoredFile, Long> {
}