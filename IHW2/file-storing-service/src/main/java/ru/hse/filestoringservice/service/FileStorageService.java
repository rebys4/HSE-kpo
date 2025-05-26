package ru.hse.filestoringservice.service;

import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;
import ru.hse.filestoringservice.entity.StoredFile;
import ru.hse.filestoringservice.repository.StoredFileRepository;

import java.io.File;
import java.io.IOException;
import java.nio.file.*;
import java.time.LocalDateTime;
import java.util.Optional;
import java.util.UUID;

@Service
@RequiredArgsConstructor
public class FileStorageService {
    private final StoredFileRepository repository;

    @Value("${file.storage.path}")
    private String storagePath;

    public StoredFile saveFile(MultipartFile file) throws IOException {
        String ext = getExtension(file.getOriginalFilename());
        String storageName = UUID.randomUUID().toString() + (ext.isEmpty() ? "" : ("." + ext));
        Path filePath = Paths.get(storagePath, storageName);
        Files.createDirectories(filePath.getParent());
        file.transferTo(filePath.toFile());

        StoredFile metadata = StoredFile.builder()
                .originalName(file.getOriginalFilename())
                .storageName(storageName)
                .storagePath(filePath.toString())
                .size(file.getSize())
                .contentType(file.getContentType())
                .uploadDate(LocalDateTime.now())
                .build();

        return repository.save(metadata);
    }

    public Optional<StoredFile> getFileMetadata(Long id) {
        return repository.findById(id);
    }

    public Path getFilePath(StoredFile metadata) {
        return Paths.get(metadata.getStoragePath());
    }

    private String getExtension(String filename) {
        if (filename == null || !filename.contains(".")) return "";
        return filename.substring(filename.lastIndexOf('.') + 1);
    }
}