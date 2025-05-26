package ru.hse.fileanalysisservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import ru.hse.fileanalysisservice.entity.FileAnalysisResult;

import java.util.Optional;

public interface FileAnalysisResultRepository extends JpaRepository<FileAnalysisResult, Long> {
    Optional<FileAnalysisResult> findByFileId(Long fileId);
}